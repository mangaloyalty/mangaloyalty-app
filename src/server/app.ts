import * as app from '.';
import * as sv from 'mangaloyalty-server';

// TODO: When adding the proxy handlers, remember provider cache!!

// Initialize the server.
sv.core.browser = new app.BrowserManager();
sv.core.resource = new app.ResourceManager();
sv.core.trace = new app.TraceManager();

// Initialize the system.
sv.bootAsync();
