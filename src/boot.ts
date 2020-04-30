import * as app from '.';
import * as areas from 'mangaloyalty-client';
import * as sv from 'mangaloyalty-server';
import cl = areas.shared;

export function bootApp(container: HTMLElement | null) {
  // Initialize the client.
  cl.api.library = new app.LibraryContext();
  cl.api.remote = new app.RemoteContext();
  cl.api.session = new app.SessionContext();
  cl.api.socket = new app.SocketContext();
  
  // Initialize the server.
  sv.core.browser = new app.BrowserManager();
  sv.core.resource = new app.ResourceManager();

  // Initialize the system.
  areas.boot.bootApp(container);
  // TODO: sv.bootApp();
}
