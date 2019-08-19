const connection = require('../infra/ConnectionFactory');

function LicencaDAO() {
  this.connection = connection;
}

LicencaDAO.prototype.getLic = function getLic(lic) {
  return new Promise((resolve, reject) => {
    this.connection.query('SELECT * FROM licencalist WHERE licencaKey = ?', [lic], (err, result) => {
      if (err) return reject(err);

      return resolve(result[0]);
    });
  });
};

// eslint-disable-next-line func-names
module.exports = new LicencaDAO();
