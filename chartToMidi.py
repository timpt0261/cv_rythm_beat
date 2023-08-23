import os
import argparse
import librosa
import librosa.display
from librosa.onset import onset_strength
from librosa.util import normalize
from midiutil.MidiFile import MIDIFile

def audio_to_midi(audio_file):
    y, sr = librosa.load(audio_file)

    # Compute the onset strength envelope
    onset_env = onset_strength(y=y, sr=sr)
    onset_env = normalize(onset_env)  # Normalize the onset envelope

    # Extract chroma features
    chroma = librosa.feature.chroma_stft(y=y, sr=sr)

    # Estimate the tempo of the audio
    tempo, _ = librosa.beat.beat_track(y=y, sr=sr)

    # Create a MIDI file
    midi = MIDIFile(1, deinterleave=False)  # Set deinterleave to False
    track = 0
    channel = 0
    time = 0
    midi.addTempo(track, time, tempo)  # Set tempo to match audio

    # Loop through time frames and add note events for detected pitches
    for frame, chroma_frame in enumerate(chroma.T):
        # Get the corresponding onset strength
        onset_strength_env = onset_env[frame]
        max_strength_index = chroma_frame.argmax()
        volume = onset_strength_env * 100  # Adjust as needed

        if volume > 20:  # Adjust threshold as needed
            midi_pitch = max_strength_index + 60  # MIDI note number
            duration = 0.1  # Duration of the note
            midi.addNote(track, channel, midi_pitch,
                         time, duration, volume=volume)

        time += librosa.frames_to_time(1, sr=sr)

    # Write the MIDI file
    output_midi_file = os.path.splitext(audio_file)[0] + '_transcription.mid'
    with open(output_midi_file, 'wb') as f:
        midi.writeFile(f)


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description="Convert audio to MIDI transcription highlighting specific pitches.")

    parser.add_argument(
        "input", help="Path to the audio file (mp3, wav, etc.)")

    args = parser.parse_args()

    audio_file = args.input

    audio_to_midi(audio_file)
