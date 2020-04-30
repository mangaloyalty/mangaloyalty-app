import * as areas from 'mangaloyalty-client';
import * as sv from 'mangaloyalty-server';
import cl = areas.shared;

export class SocketContext implements cl.ISocketContext {
  private readonly _queueHandlers: ((action: cl.ISocketAction) => void)[];
  private _isAttached?: boolean;

  constructor() {
    this._queueHandlers = [];
  }

  attach() {
    if (this._isAttached) return;
    sv.core.socket.addEventListener((action) => this._queueHandlers.forEach((queueHandler) => queueHandler(action)));
    this._isAttached = true;
  }

  createQueue() {
    return new cl.SocketQueue(this._queueHandlers);
  }
}
