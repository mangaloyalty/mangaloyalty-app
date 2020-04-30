const compression = require('compression');
const express = require('express');

// Initialize the router.
const router = express.Router();
router.use(compression());
router.use('/', express.static(`${__dirname}/public`));

// Initialize the server.
if (require.main && require.main.filename.startsWith(__dirname)) {
  const server = express();
  server.disable('x-powered-by');
  server.use(router);
  server.listen(7767, () => console.log(`Client running on http://localhost:7767/`));
} else {
  module.exports = {router};
}
