const LicencaDAO = require('../infra/LicencaDAO');

module.exports = {
  async verifyLic(req, res) {
    const { licenca } = req.params;

    if (!licenca) return res.status(400).json({ errors: ['Licenca format is invalid'] });

    const licencaObj = await LicencaDAO.getLic(licenca);

    if (!licencaObj || licencaObj.LicencaSuspensa === 1) return res.status(404).send();

    return res.status(200).json(licencaObj);
  },
};
