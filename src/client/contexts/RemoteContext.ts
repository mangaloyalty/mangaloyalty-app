import * as areas from 'mangaloyalty-client';
import * as app from '..';
import cl = areas.shared;

export class RemoteContext implements cl.IRemoteContext {
  async imageAsync(imageId: string) {
    try {
      const image = await app.proxyAsync({key: 'remote.imageAsync', imageId});
      const status = image ? 200 : 404;
      const value = image ? app.toBlob(image) : undefined;
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async popularAsync(providerName: cl.IEnumeratorProvider, pageNumber?: number | undefined) {
    try {
      const status = 200;
      const value = await app.proxyAsync({key: 'remote.popularAsync', providerName, pageNumber});
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async searchAsync(providerName: cl.IEnumeratorProvider, title: string, pageNumber?: number | undefined) {
    try {
      const status = 200;
      const value = await app.proxyAsync({key: 'remote.searchAsync', providerName, title, pageNumber});
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async seriesAsync(url: string) {
    try {
      const status = 200;
      const value = await app.proxyAsync({key: 'remote.seriesAsync', url});
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async startAsync(url: string) {
    try {
      const status = 200;
      const value = await app.proxyAsync({key: 'remote.startAsync', url});
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }
}
