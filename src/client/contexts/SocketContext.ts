import * as areas from 'mangaloyalty-client';
import cl = areas.shared;

export class SocketContext implements cl.ISocketContext {
  private readonly _queueHandlers: ((action: cl.ISocketAction) => void)[];
  private _isAttached?: boolean;

  constructor() {
    this._queueHandlers = [];
  }

  attach() {
    if (this._isAttached) return;
    window.oni?.addEventListener('socket', this._onReceive.bind(this));
    this._isAttached = true;
  }

  createQueue() {
    return new cl.SocketQueue(this._queueHandlers);
  }

  private _onReceive(action?: cl.ISocketAction) {
    if (action) {
      for (const queueHandler of this._queueHandlers) {
        queueHandler(action);
      }
    }
  }
}
