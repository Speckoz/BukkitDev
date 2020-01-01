const app = require('./config/custom-express');

const port = process.env.PORT || 3000;

app.listen(3000, () => {
  console.log(`Servidor rodando na porta ${port}`);
});
