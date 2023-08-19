import os
import argparse
import librosa
from librosa.onset import onset_strength
from librosa.util import normalize
from midiutil.MidiFile import MIDIFile

# Map pitch names to MIDI note numbers
PITCH_MAP = {'F': 65, 'G': 67, 'A': 69, 'B': 71}


def audio_to_midi(audio_file):
    # Load the audio file
    y, sr = librosa.load(audio_file)

    # Compute the onset strength envelope
    onset_env = onset_strength(y=y, sr=sr)
    onset_env = normalize(onset_env)  # Normalize the onset envelope

    # Get pitch information
    pitches, magnitudes = librosa.piptrack(y=y, sr=sr)
    pitches = pitches.T  # Transpose for processing

    print(f'Pitches {pitches} Mangitudes {magnitudes}')
    
    # Create a MIDI file
    midi = MIDIFile(1)  # 1 track
    track = 0
    channel = 0
    time = 0
    tempo = 120  # Set tempo (BPM)

    midi.addTempo(track, time, tempo)

    # Loop through time frames and add note events for specified pitches
    for frame, frame_pitches in enumerate(pitches):
        # Get the corresponding onset strength
        onset_strength_env = onset_env[frame]
        for pitch, magnitude in zip(frame_pitches, magnitudes[frame]):
            pitch_name = librosa.midi_to_note(int(round(pitch)))
            if pitch_name in PITCH_MAP and onset_strength_env > 0.1:  # Adjust threshold as needed
                midi_pitch = PITCH_MAP[pitch_name]
                duration = 0.1  # Duration of the note

                midi.addNote(track, channel, midi_pitch,
                             time, duration, volume=80)

        time += librosa.frames_to_time(1, sr=sr)

    # Write the MIDI file
    output_midi_file = os.path.splitext(audio_file)[0] + '_notes.mid'
    with open(output_midi_file, 'wb') as f:
        midi.writeFile(f)


# Replace these with your file paths
if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="Convert mp3 to MIDI highlighting specific pitches.")

    parser.add_argument(
        "input", help="Path to the audio file (mp3, wav, etc.)")

    args = parser.parse_args()

    audio_file = args.input
    audio_to_midi(audio_file)
