import * as sv from 'mangaloyalty-server';

export async function requestAsync(ev: any) {
  switch (ev?.key) {
    case 'library.listReadAsync':
      return (await sv.core.library.listReadAsync(ev.readStatus, ev.seriesStatus, ev.sortKey, ev.title));
    case 'library.listPatchAsync':
      return (await sv.core.library.listPatchAsync(ev.frequency, ev.strategy));
    case 'library.seriesCreateAsync':
      return (await sv.core.library.seriesCreateAsync(ev.url));
    case 'library.seriesDeleteAsync':
      return (await sv.core.library.seriesDeleteAsync(ev.seriesId));
    case 'library.seriesImageAsync':
      return (await sv.core.library.seriesImageAsync(ev.seriesId))?.toString('base64');
    case 'library.seriesReadAsync':
      return (await sv.core.library.seriesReadAsync(ev.seriesId));
    case 'library.seriesPatchAsync':
      return (await sv.core.library.seriesPatchAsync(ev.seriesId, ev.frequency, ev.strategy));
    case 'library.seriesUpdateAsync':
      return (await sv.core.library.seriesUpdateAsync(ev.seriesId));
    case 'library.chapterDeleteAsync':
      return (await sv.core.library.chapterDeleteAsync(ev.seriesId, ev.chapterId));
    case 'library.chapterReadAsync':
      return (await sv.core.library.chapterReadAsync(ev.seriesId, ev.chapterId))?.getData();
    case 'library.chapterPatchAsync':
      return (await sv.core.library.chapterPatchAsync(ev.seriesId, ev.chapterId, ev.isReadCompleted, ev.pageReadNumber));
    case 'library.chapterUpdateAsync':
      return (await sv.core.library.chapterUpdateAsync(ev.seriesId, ev.chapterId));
    case 'remote.imageAsync':
      return (await sv.provider.imageAsync(ev.imageId))?.toString('base64');
    case 'remote.popularAsync':
      return (await sv.core.cache.getAsync(`${ev.providerName}/${ev.pageNumber || 1}`, sv.settings.cacheDataTimeout, () => sv.provider.popularAsync(ev.providerName, ev.pageNumber)));
    case 'remote.searchAsync':
      return (await sv.core.cache.getAsync(`${ev.providerName}/${ev.title}/${ev.pageNumber || 1}`, sv.settings.cacheDataTimeout, () => sv.provider.searchAsync(ev.providerName, ev.title, ev.pageNumber)));
    case 'remote.seriesAsync':
      return (await sv.core.cache.getAsync(ev.url, sv.settings.cacheDataTimeout, () => sv.provider.seriesAsync(ev.url)));
    case 'remote.startAsync':
      return (await sv.provider.startAsync(new sv.AdaptorCache(), ev.url))?.getData();
    case 'session.list':
      return sv.core.session.getAll(ev.seriesId);
    case 'session.pageAsync':
      return (await sv.core.session.get(ev.sessionId)?.getPageAsync(ev.pageNumber))?.toString('base64');
    default:
      return;
  }
}
