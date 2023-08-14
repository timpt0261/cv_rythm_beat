import cv2
import socket
from cvzone.HandTrackingModule import HandDetector


if __name__ == '__main__':
    
    # Webcam
    width, height = 1280, 720
    cap = cv2.VideoCapture(0)
    cap.set(3, width)
    cap.set(4, height)

    # Hand Detector
    detector = HandDetector(maxHands=1, detectionCon=0.8)

    # Communications
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    serverAdressPort = ('127.0.0.1',5052)
    
    while True:
        # Get the frame from webcam
        
        success, img = cap.read()
        
        #Hands
        hands, img = detector.findHands(img)
        data = []
        # land mark values (x,y,z) * 21
        if hands: 
            # Get the first hand detected
            hand = hands[0]
            
            # Get the landmark list
            lmList = hand['lmList']
            # print(lmlist)
            for lm in  lmList:
                data.extend([lm[0], height - lm[1], lm[2]])
            sock.sendto(str.encode(str(data)), serverAdressPort)
                
        else:
            
            msg = "No hands detected"
            sock.sendto(str.encode(str(msg)), serverAdressPort)
        
        
        cv2.imshow("Image",img)
        cv2.waitKey(1)
        
