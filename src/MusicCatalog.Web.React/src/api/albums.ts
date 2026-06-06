import { api } from "./client";
import type { components } from "./schema";

export type AlbumListItem = components["schemas"]["AlbumListItemDto"];

export async function getAlbums(): Promise<AlbumListItem[]> {
    const { data, error } = await api.GET("/api/albums", {
        params: {
            query: {
                page: 1,
                pageSize: 50,
            },
        },
    });

    if (error) {
        throw error;
    }

    return data?.items ?? [];
}
