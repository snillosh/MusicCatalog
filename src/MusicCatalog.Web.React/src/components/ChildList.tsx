import {Avatar, Card, Container, Group, NavLink, Stack, Text} from "@mantine/core";
import albumCover from "../assets/ImageCompressed.png";
import type {TrackDto} from "../api/tracks.ts";

type ChildListProps = {
    selectedTrackId: string | undefined;
    tracks: TrackDto[] | undefined;
    onSelectTrack: (trackId: string) => void;
}

export function ChildList({tracks, selectedTrackId, onSelectTrack}: ChildListProps) {
    if (tracks === undefined) {
        return <Container>
            <Text>Please select an album to view it's tracks</Text>
        </Container>
    }

    return <Container>
        <Card p="lg" bg="dark.8" mt="20" shadow="lg">
            <Stack gap={4}>
                {tracks.map((track) => (
                    <NavLink
                        key={track.id}
                        active={selectedTrackId === track.id}
                        onClick={() => onSelectTrack(track.id)}
                        label={
                            <Group wrap="nowrap">
                                <Avatar
                                    src={albumCover}
                                    size={48}
                                    radius="sm"
                                />
                                <div>
                                    <Text>{track.trackNumber + ". " + track.title}</Text>
                                    <Text size="sm" c="dimmed">
                                        {track.durationSeconds}
                                    </Text>
                                </div>
                            </Group>
                        }
                    />
                ))}
            </Stack>
        </Card>
    </Container>
}
