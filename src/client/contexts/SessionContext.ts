import * as areas from 'mangaloyalty-client';
import * as app from '..';
import cl = areas.shared;

export class SessionContext implements cl.ISessionContext {
  async listAsync(seriesId?: string) {
    try {
      const status = 200;
      const value = await app.proxyAsync('session', 'list', {seriesId});
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async pageAsync(sessionId: string, pageNumber: number) {
    try {
      const image = await app.proxyAsync('session', 'pageAsync', {sessionId, pageNumber});
      const status = image ? 200 : 404;
      const value = image ? new Blob([<ArrayBuffer> image]) : undefined;
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }
}
