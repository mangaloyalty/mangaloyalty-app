var oni = (function() {
  var eventHandlers = {};
  var previousId = 0;

  return {
    addEventListener: function(eventName, handler) {
      if (eventHandlers[eventName]) {
	    throw new Error();
      } else {
        eventHandlers[eventName] = handler;
      }
    },

    dispatchAsync: function(eventName, value) {
      if (eventHandlers[eventName]) {
	    return eventHandlers[eventName](value);
      } else {
	    return undefined;
      }
    },

    removeEventListener: function(eventName) {
      if (eventHandlers[eventName]) {
        delete eventHandlers[eventName];
      }
    },

    sendAsync: function(eventName, value) {
      var callbackId = previousId++;
      var callbackName = "onicb_" + callbackId;
      return new Promise(function(resolve, reject) {
        window[callbackName] = function(success, result) {
          delete window[callbackName];
          (success ? resolve : reject)(result);
        };
        onix.request(JSON.stringify({callbackName, eventName, value}));
      });
    }
  };
})();
