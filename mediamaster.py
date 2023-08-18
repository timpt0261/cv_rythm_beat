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

    video = None
    audio = None

    try:
        yt = YouTube(url, on_complete_callback=progress_func)
    except:
        print("Connection Failure")
        return -1

    if (type == 'mp3' or type == 'both'):
        output_dir = os.path.join(output_dir, "mp3")
        audio = yt.streams.filter(only_audio=True).first()
        try:
            audio.download(output_path=output_dir, filename=yt.title)
        except:
            print(f"Unable to download {yt.title}")
            return -1
        audio.download(output_path=output_dir, filename=yt.title)
        base, ext = os.path.splitext(out_file)
        new_file = base + '.mp3'
        os.rename(out_file, new_file)

    if (type == 'mp4' or type == 'both'):
        output_dir = os.path.join(output_dir, "mp4")
        video = yt.streams.filter(
            file_extension='mp4').get_highest_resolution()

        try:
            video.download(output_path=output_dir, filename=yt.title)
        except:
            print(f"Unable to download {yt.title}")
            return -1


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

    num_links = 1
    input_value = args.input
    output_type = args.output_type

    # Find all matches in the text
    matches = findall(pattern, input_value)

    # Extract the video IDs from the matches
    video_ids = [match[0] or match[1] for match in matches]

    if (len(video_ids) == 0):
        print("No YouTube URLs Found")
        return

    # Construct the complete video URLs
    video_urls = [
        f'https://www.youtube.com/watch?v={video_id}' for video_id in video_ids]

    output_dir = os.path.join("CV_RB_2023", "Assets",
                              "Music")
    os.makedirs(output_dir, exist_ok=True)

    for url in video_urls:
        download_youtube_video(input_value, output_dir, output_type)
        # base_name = YouTube(input_value).title
        # video_path = os.path.join(output_dir, f"{base_name}.mp4")
        # if output_type == 'mp3':
        #     mp3_path = convert_to_mp3(video_path, output_dir, base_name)
        #     os.remove(video_path)
        #     print(f"Converted to MP3: {mp3_path}")
        # elif output_type == 'mp4':
        #     mp4_path = convert_to_mp4(video_path, output_dir, base_name)
        #     print(f"Converted to MP4: {mp4_path}")
        # else:
        #     mp3_path = convert_to_mp3(video_path, output_dir, base_name)
        #     mp4_path = convert_to_mp4(video_path, output_dir, base_name)
        #     print(f"Converted to MP3: {mp3_path}")
        #     print(f"Converted to MP4: {mp4_path}")

    # elif os.path.isdir(input_value):
    #     files = [f for f in os.listdir(input_value) if f.endswith('.mp4')]
    #     for file in files:
    #         file_path = os.path.join(input_value, file)
    #         base_name = os.path.splitext(file)[0]
    #         if output_type == 'mp3':
    #             mp3_path = convert_to_mp3(file_path, output_dir, base_name)
    #             print(f"Converted to MP3: {mp3_path}")
    #         elif output_type == 'mp4':
    #             mp4_path = convert_to_mp4(file_path, output_dir, base_name)
    #             print(f"Converted to MP4: {mp4_path}")
    #         else:
    #             mp3_path = convert_to_mp3(file_path, output_dir, base_name)
    #             mp4_path = convert_to_mp4(file_path, output_dir, base_name)
    #             print(f"Converted to MP3: {mp3_path}")
    #             print(f"Converted to MP4: {mp4_path}")
    # elif input_value.endswith('.txt'):
    #     with open(input_value, 'r') as txt_file:
    #         links = txt_file.readlines()
    #         links = [link.strip() for link in links]

    #     if num_links > len(links):
    #         num_links = len(links)

    #     for i in range(num_links):
    #         link = links[i]
    #         if link.startswith("https://www.youtube.com/watch?v="):
    #             download_youtube_video(link, output_dir)
    #             base_name = YouTube(link).title
    #             video_path = os.path.join(output_dir, f"{base_name}.mp4")
    #             if output_type == 'mp3':
    #                 mp3_path = convert_to_mp3(
    #                     video_path, output_dir, base_name)
    #                 os.remove(video_path)
    #                 print(f"Converted to MP3: {mp3_path}")
    #             elif output_type == 'mp4':
    #                 mp4_path = convert_to_mp4(
    #                     video_path, output_dir, base_name)
    #                 print(f"Converted to MP4: {mp4_path}")
    #             else:
    #                 mp3_path = convert_to_mp3(
    #                     video_path, output_dir, base_name)
    #                 mp4_path = convert_to_mp4(
    #                     video_path, output_dir, base_name)
    #                 print(f"Converted to MP3: {mp3_path}")
    #                 print(f"Converted to MP4: {mp4_path}")
    #         else:
    #             print(f"Invalid link format: {link}")


if __name__ == "__main__":
    main()
