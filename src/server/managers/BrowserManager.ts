import * as app from '..';
import * as sv from 'mangaloyalty-server';

export class BrowserManager implements sv.IBrowserManager {
  async pageAsync<T>(handlerAsync: (page: sv.IBrowserPage) => T | Promise<T>): Promise<T> {
    const id = await window.oni?.sendAsync('browser.createAsync');
    try {
      return await app.BrowserPage.createAsync(String(id), handlerAsync);
    } catch (error) {
      await window.oni?.sendAsync('browser.destroyAsync', {id});
      throw error;
    }
  }

  async prepareAsync() {
    await window.oni?.sendAsync('browser.bootAsync');
  }
}