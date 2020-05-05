import * as areas from 'mangaloyalty-client';
import cl = areas.shared;

// TODO: Implement me.
export class SocketContext implements cl.ISocketContext {
  private readonly _queueHandlers: ((action: cl.ISocketAction) => void)[];
  // private _isAttached?: boolean;

  constructor() {
    this._queueHandlers = [];
  }

  attach() {
    // if (this._isAttached) return;
    // sv.core.socket.addEventListener((action) => this._queueHandlers.forEach((queueHandler) => queueHandler(action)));
    // this._isAttached = true;
  }

  createQueue() {
    return new cl.SocketQueue(this._queueHandlers);
  }
}
