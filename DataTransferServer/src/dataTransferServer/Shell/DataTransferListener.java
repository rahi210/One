package dataTransferServer.Shell;

import java.util.Observable;
import java.util.Observer;

public class DataTransferListener implements Observer {

    public void update(Observable o, Object arg) {
        System.out.println("RunThreadËÀ»ú");
        DataTransfer run = new DataTransfer();
        run.addObserver(this);
        new Thread(run).start();
        System.out.println("RunThreadÖØÆô");
    }
}
