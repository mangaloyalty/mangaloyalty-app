import * as areas from 'mangaloyalty-client';
import * as app from '..';
import cl = areas.shared;

export class LibraryContext implements cl.ILibraryContext {
  async listReadAsync(readStatus: cl.IEnumeratorReadStatus, seriesStatus: cl.IEnumeratorSeriesStatus, sortKey: cl.IEnumeratorSortKey, title?: string | undefined) {
    try {
      const status = 200;
      const value = await app.proxyAsync('library', 'listReadAsync', {readStatus, seriesStatus, sortKey, title});
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async seriesCreateAsync(url: string) {
    try {
      const id = await app.proxyAsync('library', 'seriesCreateAsync', {url});
      const status = 200;
      const value = {id};
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async seriesDeleteAsync(seriesId: string) {
    try {
      const success = await app.proxyAsync('library', 'seriesDeleteAsync', {seriesId});
      const status = success ? 200 : 404;
      return {status};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  seriesDumpAsync() {
    return Promise.resolve({status: 500});
  }

  async seriesImageAsync(seriesId: string) {
    try {
      const image = await app.proxyAsync('library', 'seriesImageAsync', {seriesId});
      const status = image ? 200 : 404;
      const value = image ? new Blob([<ArrayBuffer> image]) : undefined;
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async seriesReadAsync(seriesId: string) {
    try {
      const series = await app.proxyAsync('library', 'seriesReadAsync', {seriesId});
      const status = series ? 200 : 404;
      const value = series;
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async seriesPatchAsync(seriesId: string, frequency: cl.IEnumeratorFrequency, strategy: cl.IEnumeratorStrategy) {
    try {
      const success = await app.proxyAsync('library', 'seriesPatchAsync', {seriesId, frequency, strategy});
      const status = success ? 200 : 404;
      return {status};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async seriesUpdateAsync(seriesId: string) {
    try {
      const success = await app.proxyAsync('library', 'seriesUpdateAsync', {seriesId});
      const status = success ? 200 : 404;
      return {status};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async chapterDeleteAsync(seriesId: string, chapterId: string) {
    try {
      const success = await app.proxyAsync('library', 'chapterDeleteAsync', {seriesId, chapterId});
      const status = success ? 200 : 404;
      return {status};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async chapterReadAsync(seriesId: string, chapterId: string) {
    try {
      const session = await app.proxyAsync('library', 'chapterReadAsync', {seriesId, chapterId});
      const status = session ? 200 : 404;
      const value = session ? session.getData() : undefined;
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async chapterPatchAsync(seriesId: string, chapterId: string, isReadCompleted?: boolean | undefined, pageReadNumber?: number | undefined) {
    try {
      const success = await app.proxyAsync('library', 'chapterPatchAsync', {seriesId, chapterId, isReadCompleted, pageReadNumber});
      const status = success ? 200 : 404;
      return {status};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async chapterUpdateAsync(seriesId: string, chapterId: string) {
    try {
      const success = await app.proxyAsync('library', 'chapterUpdateAsync', {seriesId, chapterId});
      const status = success ? 200 : 404;
      return {status};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }
}
