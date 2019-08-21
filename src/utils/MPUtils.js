const mercadopago = require('../services/MPService');

module.exports.Purchase = async (plugin) => {
  const { ID, NomePlugin, PrecoPlugin } = plugin;

  // Create a preference structure
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

  return mercadopago.preferences.create(preference)
    .then((response) => response.response)
    .then((response) => {
      console.log('Pagamento CRIADO');
      // console.log(response);
      return {
        id: response.id,
        link: response.sandbox_init_point,
        description: response.items[0].description,
      };
    })
    .catch((error) => {
      console.log(error);
      return { error };
    });
};
