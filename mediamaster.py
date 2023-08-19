import os
import argparse
from pytube import YouTube
from re import findall
from moviepy.video.io.VideoFileClip import VideoFileClip

pattern = r'https://(?:www\.)?youtu\.be/(\w+)|https://(?:www\.)?youtube\.com/watch\?v=(\w+)'


def progress_func(stream, chunk, remaining):
    total_size = stream.filesize
    downloaded_size = total_size - remaining
    percentage = (downloaded_size / total_size) * 100
    print(f"Downloading: {percentage:.2f}%", end='\r')


def download_youtube_video(url, output_dir, type):
    try:
        yt = YouTube(url, on_complete_callback=progress_func)
    except:
        print(f"Connection Failure for {url}")
        return -1

    if (type == 'mp3' or type == 'both'):
        output_subdir = "mp3"
        audio = yt.streams.filter(only_audio=True).first()
        try:
            audio.download(output_path=output_dir, filename=yt.title)
            base, ext = os.path.splitext(audio.default_filename)
            new_file = os.path.join(output_dir, output_subdir, f"{base}.mp3")
            os.rename(audio.default_filename, new_file)
        except:
            print(f"Unable to download {yt.title}")

    if (type == 'mp4' or type == 'both'):
        output_subdir = "mp4"
        video = yt.streams.filter(
            file_extension='mp4').get_highest_resolution()
        try:
            video.download(output_path=output_dir, filename=yt.title)
        except:
            print(f"Unable to download {yt.title}")


def main():
    parser = argparse.ArgumentParser(
        description="Convert YouTube links to MP3 or MP4.")
    # parser.add_argument("-m", type=int, default=1,
    #                     help="Number of links to process")
    parser.add_argument(
        "input", help="YouTube URL, directory path, or .txt file containing links")
    parser.add_argument("output_type", choices=[
                        'mp3', 'mp4', 'both'], help="Output file type")
    args = parser.parse_args()

    # num_links = 1  # Not needed anymore
    input_value = args.input
    output_type = args.output_type

    # Find all matches in the text
    matches = findall(pattern, input_value)

    # Extract the video IDs from the matches
    video_ids = [match[0] or match[1] for match in matches]

    if len(video_ids) == 0:
        print("No YouTube URLs Found")
        return

    # Construct the complete video URLs
    video_urls = [
        f'https://www.youtube.com/watch?v={video_id}' for video_id in video_ids]

    output_dir = os.path.join("music")
    os.makedirs(output_dir, exist_ok=True)

    for url in video_urls:
        download_youtube_video(url, output_dir, output_type)


if __name__ == "__main__":
    main()
