import * as app from '.';
import * as sv from 'mangaloyalty-server';

// Initialize the server.
sv.core.browser = new app.BrowserManager();
sv.core.resource = new app.ResourceManager();
sv.core.socket = new app.SocketManager();
sv.core.trace = new app.TraceManager();

// Initialize the system.
sv.bootAsync().then((router) => {
  window.oni?.addEventListener('request', async (ev: any) => {
    const result = await router.execAsync(ev.method, ev.path, ev.context);
    if (Buffer.isBuffer(result.content)) {
      const base64 = result.content.toString('base64');
      const headers = result.headers;
      const statusCode = result.statusCode;
      return {base64, headers, statusCode};
    } else {
      return result;
    }
  });
});
