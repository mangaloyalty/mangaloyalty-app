import * as app from '.';
import * as sv from 'mangaloyalty-server';

// Initialize the request handler.
window.oni?.addEventListener('request', async (ev?: any) => {
  if (!ev) return;
  return await app.requestAsync(ev);
});

// Initialize the server.
sv.core.browser = new app.BrowserManager();
sv.core.resource = new app.ResourceManager();
sv.core.socket = new app.SocketManager();
sv.core.trace = new app.TraceManager();

// Initialize the system.
sv.bootAsync();
