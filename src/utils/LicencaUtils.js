const Logger = require('../config/Logger');

module.exports.verifyIP = (req, licencaObj) => {
  if (!licencaObj) return false;

  let ip = req.headers['x-forwarded-for'] || req.connection.remoteAddress;

  if (ip.substr(0, 7) === '::ffff:') ip = ip.substr(7);

  // Local IP's and Licence IP
  if (
    ip === licencaObj.IPPermitido ||
    ip === '::1' ||
    ip.startsWith('192.168') ||
    ip.startsWith('127.0')
  ) {
    return true;
  }

  Logger.info('Tentativa de usar a licença de um IP não permitido!', {
    ip: req.connection.remoteAddress,
    key: licencaObj.LicencaKey,
    data: new Date(),
  });

  return false;
};

module.exports.verifyLic = (req, licencaObj) => {
  if (!licencaObj || licencaObj.LicencaSuspensa === 1) {
    Logger.info('Tentativa de usar uma licença invalida!', {
      ip: req.connection.remoteAddress,
      key: licencaObj.LicencaKey,
      data: new Date(),
    });

    return false;
  }
  return true;
};
