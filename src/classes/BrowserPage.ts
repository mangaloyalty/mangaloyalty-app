import * as app from '..';
import * as sv from 'mangaloyalty-server';

export class BrowserPage implements sv.IBrowserPage {
  private readonly _handlers: ((response: sv.IBrowserResponse) => void)[];
  private readonly _id: string;

  private constructor(id: string) {
    this._handlers = [];
    this._id = id;
  }

  static async createAsync<T>(id: string, handlerAsync: (page: app.BrowserPage) => Promise<T> | T) {
    const page = new app.BrowserPage(id);
    const handler = (value?: string) => page._onReceive(value);
    const eventName = `browser.${id}`;
    try {
      app.native().addEventListener(eventName, handler);
      return await handlerAsync(page);
    } catch (error) {
      app.native().removeEventListener(eventName, handler);
      throw error;
    }
  }

  addEventListener(handler: (response: sv.IBrowserResponse) => void) {
    this._handlers.push(handler);
  }

  async evaluateAsync<T extends (...args: any[]) => any>(handler: T) {
    const id = this._id;
    const invoke = String(handler);
    return await app.native().sendAsync('browser.evaluateAsync', {id, invoke}) as ReturnType<T>;
  }

  async navigateAsync(url: string) {
    const id = this._id;
    await app.native().sendAsync('browser.navigateAsync', {id, url});
  }

  async waitForNavigateAsync() {
    const id = this._id;
    await app.native().sendAsync('browser.waitForNavigateAsync', {id});
  }

  private _onReceive(url?: string) {
    if (url) {
      for (const handler of this._handlers) try {
        handler(new app.BrowserResponse(this._id, url));
      } catch (error) {
        sv.writeError(error);
      }
    }
  }
}
