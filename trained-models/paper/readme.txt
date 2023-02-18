yolov4-obj_9000.weights: 
	Was trained for 9000 iterations using train and test as specified in the paper.
	.cfg file, test.txt, train.txt, obj.data and obj.names were also included. If you want to rerun you need to modify their names 
	by removing all _9000 suffixes.
	Sample run command: 
		.\darknet.exe detector train build\darknet\x64\data\obj.data cfg\yolov4-obj.cfg build\darknet\x64\yolov4.conv.137
