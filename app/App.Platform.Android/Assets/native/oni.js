var oni = (function() {
  var eventHandlers = {};
  var previousId = 0;

  return {
    addEventListener: function(key, handler) {
      if (eventHandlers[key]) {
	    throw new Error();
      } else {
        eventHandlers[key] = handler;
      }
    },

    dispatchAsync: function(key, value) {
      if (eventHandlers[key]) {
	    return eventHandlers[key](value);
      } else {
	    return undefined;
      }
    },

    removeEventListener: function(key) {
      if (eventHandlers[key]) {
        delete eventHandlers[key];
      }
    },

    sendAsync: function(key, value) {
      var callbackId = previousId++;
      var callback = "onicb_" + callbackId;
      return new Promise(function(resolve, reject) {
        window[callback] = function(success, result) {
          delete window[callback];
          (success ? resolve : reject)(result);
        };
        onix.fromJs(JSON.stringify({callback, key, value}));
      });
    }
  };
})();
