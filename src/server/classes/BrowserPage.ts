import * as sv from 'mangaloyalty-server';

export class BrowserPage implements sv.IBrowserPage {
  private readonly _id: string;

  constructor(id: string) {
    this._id = id;
  }

  async evaluateAsync<T extends (...args: any[]) => any>(handler: T) {
    const id = this._id;
    const invoke = String(handler);
    return await window.oni?.sendAsync('browser.evaluateAsync', {id, invoke}) as ReturnType<T>;
  }

  async navigateAsync(url: string) {
    const id = this._id;
    await window.oni?.sendAsync('browser.navigateAsync', {id, url});
  }

  async responseAsync(url: string): Promise<Buffer> {
    const id = this._id;
    const result = await window.oni?.sendAsync('browser.responseAsync', {id, url});
    return new Buffer(String(result), 'base64');
  }

  async waitForNavigateAsync() {
    const id = this._id;
    await window.oni?.sendAsync('browser.waitForNavigateAsync', {id});
  }
}
