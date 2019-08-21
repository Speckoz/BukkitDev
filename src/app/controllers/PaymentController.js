const mercadopago = require('../../services/MPService');
const PluginDAO = require('../infra/PluginDAO');

module.exports = {
  async createPayment(req, res) {
    const { pluginId } = req.body;
    const plugin = await PluginDAO.getPlugin(pluginId);

    // Create a preference structure

    const {
      ID, NomePlugin,
      PrecoPlugin,
    } = plugin;

    const item = {
      id: ID,
      title: NomePlugin,
      description: ID.toString(),
      quantity: 1,
      currency_id: 'BRL',
      unit_price: parseFloat(PrecoPlugin),
    };

    const preference = {
      items: [
        item,
      ],
    };

    mercadopago.preferences.create(preference)
      .then((response) => response.response)
      .then((response) => {
        console.log('Pagamento CRIADO');
        console.log(response);
        return res.status(201).json({
          id: response.id,
          link: response.sandbox_init_point,
          description: response.items[0].description,
        });
      })
      .catch((error) => {
        console.log(error);
        return res.status(500).json(error);
      });
  },
};
