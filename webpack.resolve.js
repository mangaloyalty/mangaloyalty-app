module.exports = {
  resolve: {
    alias: {
      'express': `${__dirname}/dist/webpack/express`,
      'express-openapi-json': `${__dirname}/dist/webpack/express-openapi-json`,
      'fs-extra': `${__dirname}/dist/webpack/fs-extra`,
      'puppeteer-core': `${__dirname}/dist/webpack/puppeteer-core`,
      'socket.io': `${__dirname}/dist/webpack/socket.io`,
      'socket.io-client': `${__dirname}/dist/webpack/socket.io-client`,
      'winston': `${__dirname}/dist/webpack/winston`,
      'zip-stream': `${__dirname}/dist/webpack/zip-stream`
    }
  }
};
