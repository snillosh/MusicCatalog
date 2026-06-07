import {api} from "./client";
import type {components} from "./schema";

export type AlbumDto = components["schemas"]["AlbumDto"];
export type ImportAlbumRequest = components["schemas"]["ImportAlbumRequest"];

export async function importAlbum(
    request: ImportAlbumRequest
): Promise<AlbumDto> {
    const {data, error} = await api.POST("/api/albums/import", {
        body: request,
    });

    if (error) {
        throw error;
    }

    return data;
}
