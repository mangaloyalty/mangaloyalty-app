import * as areas from 'mangaloyalty-client';
import * as sv from 'mangaloyalty-server';
import cl = areas.shared;

export class SessionContext implements cl.ISessionContext {
  async listAsync(seriesId?: string) {
    try {
      const status = 200;
      const value = sv.core.session.getAll(seriesId);
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }

  async pageAsync(sessionId: string, pageNumber: number) {
    try {
      const image = await sv.core.session.get(sessionId)?.getPageAsync(pageNumber);
      const status = image ? 200 : 404;
      const value = image ? new Blob([<ArrayBuffer> image]) : undefined;
      return {status, value};
    } catch (error) {
      const status = 500;
      return {status, error};
    }
  }
}
