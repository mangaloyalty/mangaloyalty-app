module.exports = {
  resolve: {
    alias: {
      'express-openapi-json': `${__dirname}/dist/webpack/express-openapi-json`,
      'fs-extra': `${__dirname}/dist/webpack/fs-extra`,
      'puppeteer-core': `${__dirname}/dist/webpack/puppeteer-core`,
      'winston': `${__dirname}/dist/webpack/winston`,
      'zip-stream': `${__dirname}/dist/webpack/zip-stream`
    }
  }
};
