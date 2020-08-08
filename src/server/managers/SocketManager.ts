import * as sv from 'mangaloyalty-server';

export class SocketManager implements sv.ISocketManager {
  private _previous = Promise.resolve();
  
  addEventListener() {
    throw new Error();
  }

  removeEventListener() {
    throw new Error();
  }

  emit(action: sv.ISocketAction) {
    this._previous = this._previous.then(() => window.oni?.sendAsync('socket.emitAsync', action));
  }
}
