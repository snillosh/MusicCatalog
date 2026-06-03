import type {Track} from "../models/track.ts";
import {Avatar, Card, Container, Group, NavLink, Stack, Text} from "@mantine/core";
import albumCover from "../assets/ImageCompressed.png";

type ChildListProps = {
    selectedTrackId: number | undefined;
    tracks: Track[] | undefined;
    onSelectTrack: (trackId: number) => void;
}

export function ChildList({tracks, selectedTrackId, onSelectTrack} : ChildListProps)
{
    if (tracks === undefined)
    {
        return <Container>
            <Text>Please select an album to view it's tracks</Text>
        </Container>
    }

    return <Container>
        <Card p="lg" bg="dark.8" mt="20">
            <Stack gap={4}>
                {tracks.map((track) => (
                    <NavLink
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
                                    <Text>{track.name}</Text>
                                    <Text size="sm" c="dimmed">
                                        {track.duration}
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
