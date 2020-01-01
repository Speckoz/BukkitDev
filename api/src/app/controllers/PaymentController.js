const PluginDAO = require('../infra/PluginDAO');
const MPUtils = require('../../utils/MPUtils');

module.exports = {
  async createPayment(req, res) {
    const { pluginId } = req.body;
    const plugin = await PluginDAO.getPlugin(pluginId);

    const purchase = await MPUtils.Purchase(plugin);

    if (purchase.error) {
      return res.status(500).json(purchase);
    }

    return res.status(201).json(purchase);
  },
};
