module.exports = {
  resolve: {
    alias: {
      'fs-extra': `${__dirname}/dist/webpack/fs-extra`,
      'puppeteer-core': `${__dirname}/dist/webpack/puppeteer-core`,
      'winston': `${__dirname}/dist/webpack/winston`,
      'zip-stream': `${__dirname}/dist/webpack/zip-stream`
    }
  }
};
