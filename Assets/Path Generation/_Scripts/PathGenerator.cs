using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionDirection {Left, Straight, Right}
public class PathGenerator : MonoBehaviour
{
    [SerializeField]int pathLeght = 10;
    [SerializeField]List<PathSection> pathSectionPrefabs;
    [SerializeField]PathSection startSection;
    public List<PathSection> availablePathSections = new();
    PathSection currentSection;
    int pathTilt = 0;
    int currentAngle;
    Queue<PathSection> activePaths = new();
    void Start()
    {
        availablePathSections.AddRange(pathSectionPrefabs);
        //ComposePath();
        StartCoroutine(GenerateTest());
    }
    void ComposePath()
    {
        for(int i = 0;i < pathLeght; i++)
        {
            GenerateSection();
        }
    }
    void GenerateSection()
    {
        if(currentSection == null)
        {
            currentSection = Instantiate(startSection);
            activePaths.Enqueue(currentSection);
            currentSection.transform.eulerAngles = new Vector3(0,90,0);
        }
        CalculateAvailablePaths();
        PathSection nextSection = Instantiate(availablePathSections[Random.Range(0,availablePathSections.Count)]);    
        
        
        switch (currentSection.GetDirection())
        {
            case TransitionDirection.Left:
                TransformSection(-60,currentSection,nextSection);
                pathTilt --;
                break;
            case TransitionDirection.Straight:
                TransformSection(0,currentSection,nextSection);
                break;
            case TransitionDirection.Right:
                TransformSection(60,currentSection,nextSection);
                pathTilt ++;
                break;
        }
        if(!activePaths.Contains(currentSection))
            activePaths.Enqueue(currentSection);
        
        if(activePaths.Count > 3)
        {
            Destroy(activePaths.Peek().gameObject);
            activePaths.Dequeue();
        }
    }
    void TransformSection(int angle, PathSection curSection, PathSection nextSection)
    {
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        nextSection.transform.rotation = curSection.transform.rotation * rotation;

        Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * 1.73205080757f;
        Debug.Log($"Rotation: {nextSection.transform.rotation.eulerAngles}, Offset: {offset}");

        nextSection.transform.position = curSection.transform.position + offset;

        currentSection = nextSection;
    }

    void CalculateAvailablePaths()
    {
        Debug.Log($"pathTilt: {pathTilt}");
        Debug.Log("current Directions: "+ currentSection.GetDirection());
        if (pathTilt < 0)
        {
            availablePathSections.RemoveAll(section => section.GetDirection() == TransitionDirection.Left);
            Debug.Log("kicked left");
        }
        else if (pathTilt > 0)
        {
            availablePathSections.RemoveAll(section => section.GetDirection() == TransitionDirection.Right);
            Debug.Log("kicked right");
        }
        else
        {
            availablePathSections = new List<PathSection>(pathSectionPrefabs);
            Debug.Log("no pathTilt change");
        }
    }
    IEnumerator GenerateTest()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            GenerateSection();
        }
    }
}