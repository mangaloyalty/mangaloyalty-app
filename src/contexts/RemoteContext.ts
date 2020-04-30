import * as areas from 'mangaloyalty-client';
import * as sv from 'mangaloyalty-server';
import cl = areas.shared;

export class RemoteContext implements cl.IRemoteContext {
  async imageAsync(imageId: string) {
    try {
      const image = await sv.provider.imageAsync(imageId);
      const status = image ? 200 : 404;
      const value = image ? new Blob([<ArrayBuffer> image]) : undefined;
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async popularAsync(providerName: cl.IEnumeratorProvider, pageNumber?: number | undefined) {
    try {
      const key = `${providerName}/${pageNumber || 1}`;
      const status = 200;
      const value = await sv.core.cache.getAsync(key, sv.settings.cacheDataTimeout, () => sv.provider.popularAsync(providerName, pageNumber));
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async searchAsync(providerName: cl.IEnumeratorProvider, title: string, pageNumber?: number | undefined) {
    try {
      const key = `${providerName}/${title}/${pageNumber || 1}`;
      const status = 200;
      const value = await sv.core.cache.getAsync(key, sv.settings.cacheDataTimeout, () => sv.provider.searchAsync(providerName, title, pageNumber));
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async seriesAsync(url: string) {
    try {
      const key = url;
      const status = 200;
      const value = await sv.core.cache.getAsync(key, sv.settings.cacheDataTimeout, () => sv.provider.seriesAsync(url));
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async startAsync(url: string) {
    try {
      const session = await sv.provider.startAsync(new sv.AdaptorCache(), url);
      const status = 200;
      const value = session.getData();
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }
}
