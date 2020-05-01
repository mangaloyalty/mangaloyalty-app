(function() {
  console.debug = function(message) {
    webkit.messageHandlers.native.postMessage('d:' + message);
  };

  console.error = function(message) {
    webkit.messageHandlers.native.postMessage('e:' + message);
  };

  console.info = function(message) {
    webkit.messageHandlers.native.postMessage('i:' + message);
  };

  console.log = function(message) {
    webkit.messageHandlers.native.postMessage('l:' + message);
  };

  console.warn = function(message) {
    webkit.messageHandlers.native.postMessage('w:' + message);
  };

  window.onerror = function(message, fileName, lineNumber, columnNumber) {
    console.error('w:%s (%d:%d)', message, lineNumber, columnNumber);
  };
})();
