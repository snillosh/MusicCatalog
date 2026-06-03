import type {Track} from "./track.ts";

export type Album = {
    id: number;
    name: string;
    artist: string;
    tracks: Track[];
    releaseYear: number;
    rating: string;
}
