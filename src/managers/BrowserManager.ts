import * as sv from 'mangaloyalty-server';

export class BrowserManager implements sv.IBrowserManager {
  pageAsync<T>(_handlerAsync: (page: sv.IBrowserPage) => T | Promise<T>): Promise<T> {
    throw new Error('TODO');
  }

  prepareAsync(): Promise<void> {
    throw new Error('TODO');
  }
}
