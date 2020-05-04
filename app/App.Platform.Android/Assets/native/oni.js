var oni = (function() {
  var eventHandlers = {};
  var previousId = 0;

  return {
    addEventListener: function(eventName, handler) {
      if (eventHandlers[eventName]) {
        eventHandlers[eventName].push(handler);
      } else {
        eventHandlers[eventName] = [handler];
      }
    },

    dispatchEvent: function(eventName, value) {
      if (eventHandlers[eventName]) {
        for (var i = 0; i < eventHandlers[eventName].length; i++) {
          eventHandlers[eventName][i](value);
        }
      }
    },

    removeEventListener: function(eventName, handler) {
      if (eventHandlers[eventName]) {
        var index = eventHandlers[eventName].indexOf(handler);
        if (index !== -1) eventHandlers[eventName].splice(index, 1);
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
