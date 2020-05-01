(function() {
  console.debug = createFormatter(console.debug);
  console.error = createFormatter(console.error);
  console.info = createFormatter(console.info);
  console.log = createFormatter(console.log);
  console.warn = createFormatter(console.warn);

  function createFormatter(fn) {
    return function() {
      fn.call(this, format.apply(this, arguments));
    };
  }

  function format() {
    switch (typeof arguments[0]) {
      case 'object':
        return arguments[0] && JSON.stringify(arguments[0]) || '';
      case 'string':
        return formatString(Array.prototype.slice.call(arguments));
      case 'undefined':
        return '';
      default:
        return String(arguments[0]);
    }
  }

  function formatString(args) {
    return [args.shift().replace(/%\.?([0-9]+)?(o|O|d|i|s|f)/g, function (match, fractionDigits, command) {
      if (args.length === 0) return match;
      var value = args.shift();
      return formatStringValue(fractionDigits, command, value) || match;
    })].concat(args).join(' ');
  }
  
  function formatStringValue(fractionDigits, command, value) {
    if (command === 'o' || command === 'O') {
      return JSON.stringify(value);
    } else if (command === 'd' || command === 'i') {
      return parseInt(value, 10).toFixed(fractionDigits || 0);
    } else if (command === 'f') {
      return parseFloat(value, 10).toFixed(fractionDigits || 0);
    } else if (command === 's') {
      return value;
    } else {
      return undefined;
    }
  }
})();
