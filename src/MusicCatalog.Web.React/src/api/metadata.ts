import type {components} from "./schema.ts";
import {api} from "./client.ts";


export type AlbumGroupSearchResult = components["schemas"]["AlbumGroupSearchResult"];
export type AlbumImportPreview = components["schemas"]["AlbumImportPreview"];

export async function getAlbumMetadataByQuery(query: string): Promise<AlbumGroupSearchResult[]> {
    const {data, error} = await api.GET("/api/metadata/{query}", {
        params: {
            path: {
                query,
            },
        },
    });

    if (error) {
        throw error;
    }

    return data ?? [];
}

export async function getAlbumMetadataById(id: string): Promise<AlbumImportPreview> {
    const {data, error} = await api.GET("/api/metadata/{id}", {
        params: {
            path: {
                id,
            },
        },
    });

    if (error) {
        throw error;
    }

    return data;
}


