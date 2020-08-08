import * as areas from 'mangaloyalty-client';
import cl = areas.shared;

// Initialize the back handler.
window.oni?.addEventListener('backbutton', () => {
  if (cl.core.dialog.isChildVisible || cl.core.screen.loadCount) return;
  if (cl.core.screen.views.length > 1) return cl.core.screen.leaveAsync();
  return window.oni?.sendAsync('shell.minimizeApp');
});

// Initialize the system.
areas.boot.boot(document.getElementById('container'));
setTimeout(() => window.oni?.sendAsync('shell.hideSplashScreen'), 1000);
