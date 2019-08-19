const LicencaDAO = require('../infra/LicencaDAO');

module.exports = {
  async verifyLic(req, res) {
    const { licenca } = req.params;

    if (!licenca) return res.status(400).json({ errors: ["Licenca can't be empty"] });

    return res.status(200).json(licenca);
  },
};
