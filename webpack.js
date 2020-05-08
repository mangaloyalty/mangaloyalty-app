module.exports = Object.assign(require('./webpack.resolve'), { 
  entry: {client: './dist/client/app', server: './dist/server/app'},
  output: {filename: '[name].min.js', path: `${__dirname}/public`},
  performance: {hints: false},
  mode: 'production'
});
