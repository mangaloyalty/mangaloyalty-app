import * as sv from 'mangaloyalty-server';

export class TraceManager implements sv.ITraceManager {
  private _previous = Promise.resolve();

  error(error: any): void {
    if (error instanceof Error) this.info(error.stack || error.message);
    else if (error && String(error)) this.error(new Error(String(error)));
    else this.error(new Error());
  }

  info(message: string): void {
    this._previous = this._previous.then(() => window.oni?.sendAsync('trace.infoAsync', {message}));
  }
}
