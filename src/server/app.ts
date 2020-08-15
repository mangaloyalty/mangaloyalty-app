import * as app from '.';
import * as api from 'express-openapi-json';
import * as sv from 'mangaloyalty-server';

// Initialize the server.
sv.core.browser = new app.BrowserManager();
sv.core.resource = new app.ResourceManager();
sv.core.trace = new app.TraceManager();
sv.settings.actionWaitTimeout = 25000;

// Initialize the system.
sv.bootAsync().then((router) => {
  window.oni?.addEventListener('request', async (model: any) => {
    const result = await router.execAsync(model.method, model.path, new api.Context(model.context));
    if (Buffer.isBuffer(result.content)) {
      const content64 = result.content.toString('base64');
      const headers = result.headers;
      const statusCode = result.statusCode;
      return {content64, headers, statusCode};
    } else {
      const content = result.content;
      const headers = result.headers;
      const statusCode = result.statusCode;
      return {content, headers, statusCode};
    }
  });
});
