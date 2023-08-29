import cv2
import socket
from cvzone.HandTrackingModule import HandDetector

# for thumbs up detection
import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision

# Create an GestureRecognizer object.
base_options = python.BaseOptions(model_asset_path='gesture_recognizer.task')
options = vision.GestureRecognizerOptions(base_options=base_options)
recognizer = vision.GestureRecognizer.create_from_options(options)

# if the ML model detects gesture with more than 40% probability we will consider it
thumb_up_score_threshold = 0.4

if __name__ == '__main__':

    # Webcam
    width, height = 640, 360
    cap = cv2.VideoCapture(0)
    cap.set(3, width)
    cap.set(4, height)

    # Hand Detector
    detector = HandDetector(maxHands=2, detectionCon=0.8)

    # Communications
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    serverAdressPort = ('127.0.0.1', 5052)

    while True:
        # Get the frame from webcam

        success, img = cap.read()
        img_has_thumb_up = 0

        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=img)
        recognition_result = recognizer.recognize(mp_image)
        if (len(recognition_result.gestures) > 0 and len(recognition_result.gestures[0]) > 0):
            print("A")
            top_gesture = recognition_result.gestures[0][0]
            if (top_gesture.category_name == 'Thumb_Up' and top_gesture.score >= thumb_up_score_threshold):
                print("B")
                img_has_thumb_up = 1

        # Hands
        hands, img = detector.findHands(img)
        data = []
        # land mark values (x,y,z) * 21
        if hands:
            # Get the first hand_01 detected
            hand_01 = hands[0]
            # Get the landmark list
            lmList = hand_01['lmList']
            for lm in lmList:
                data.extend([lm[0], height - lm[1], lm[2]])

            if (len(hands) == 2):
                # Get the landmark list
                hand_02 = hands[1]
                lmList = hand_02['lmList']
                for lm in lmList:
                    data.extend([lm[0], height - lm[1], lm[2]])
            data.append(img_has_thumb_up)
            # print(str.encode(str(data)))
            sock.sendto(str.encode(str(data)), serverAdressPort)
        else:

            msg = "None"
            sock.sendto(str.encode(str(msg)), serverAdressPort)

        cv2.imshow("Hand Detector", img)
        cv2.waitKey(1)
