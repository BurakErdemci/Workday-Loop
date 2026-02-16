using UnityEngine;
using UnityEngine.EventSystems;

public class FolderDropZone : MonoBehaviour, IDropHandler
{
    public string folderCategory;
    public FileSortingTask taskController;
    
    [Header("Sesler")]
    public AudioSource folderSource;
    public AudioClip dropSound; 
    public void OnDrop(PointerEventData eventData)
    {
        
        if (eventData.pointerDrag != null)
        {
            DraggableFile file = eventData.pointerDrag.GetComponent<DraggableFile>();

            if (file != null)
            {
            
                bool isSabotageAllowed = DayCycleManager.currentDay >= 2;

                if (isSabotageAllowed || file.fileCategory == folderCategory)
                {
                 
                    file.wasSuccessfullyDropped = true;

                
                    if (taskController != null)
                    {
                        taskController.OnFileDropped(file.fileCategory, folderCategory);
                    }
                    
                    if (file != null && folderSource != null) {
                        folderSource.PlayOneShot(dropSound, 0.5f);
                    }

                   
                    eventData.pointerDrag.SetActive(false);
                    Debug.Log($"[FOLDER] {file.fileCategory} dosyası {folderCategory} içine kabul edildi.");
                }
                else
                {
                   
                    Debug.Log($"[FOLDER] Bugün sadece dürüst çalışma. {file.fileCategory} dosyası {folderCategory} içine konulamaz!");
                }
            }
        }
        
    } 

    
    public void OnDrag(PointerEventData eventData) { }   
}