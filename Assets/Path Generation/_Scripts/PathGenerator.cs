using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public enum TransitionDirection {Left, Straight, Right}
public enum PathDirection {Left, Straight, Right}
public class PathGenerator : MonoBehaviour
{
    [Header("For Test Purposes")]
    [SerializeField]int pathLeght = 10;


    [Header("")]

    [SerializeField]List<PathSection> pathSectionPrefabs;
    [SerializeField]PathSection startSection;
    public List<PathSection> availablePathSections = new(); //temp public
    List<PathSection> leftSections;
    List<PathSection> rightSections;
    PathSection currentSection;
    Queue<PathSection> activePaths = new();
    float hexLength = 0.8660254f;
    public Vector3 previousOffset = Vector3.zero; //temp public
    public PathDirection pathDirection;
    void Start()
    {
        availablePathSections.AddRange(pathSectionPrefabs);
        leftSections = pathSectionPrefabs.Where(obj => obj.GetDirection() == TransitionDirection.Left).ToList();
        rightSections = pathSectionPrefabs.Where(obj => obj.GetDirection() == TransitionDirection.Right).ToList();

        //ComposePath();
        StartCoroutine(GenerateTest());
    }
    void ComposePath()
    {
        for(int i = 0;i < pathLeght; i++)
            GenerateSection();
    }
    void GenerateSection()
    {
        if(currentSection == null)
        {
            currentSection = Instantiate(startSection);
            activePaths.Enqueue(currentSection);
            currentSection.transform.eulerAngles = new Vector3(0,90,0);
            pathDirection = PathDirection.Straight;
            AddToFolder(currentSection);
            return;
        }
        
        PathSection nextSection = Instantiate(availablePathSections[Random.Range(0,availablePathSections.Count)]);    
        AddToFolder(nextSection);
        
        int angle = 0;
        switch (currentSection.GetDirection())
        {
            case TransitionDirection.Left:
                angle = -60;
                if(pathDirection ==PathDirection.Right)
                    pathDirection = PathDirection.Straight;
                else
                    pathDirection = PathDirection.Left;
                break;
            case TransitionDirection.Right:
                angle = 60;
                if(pathDirection == PathDirection.Left)
                    pathDirection = PathDirection.Straight;
                else
                    pathDirection = PathDirection.Right;
                break;
        }
        TransformSection(angle,currentSection,nextSection);
        CalculateAvailablePaths();

        if(!activePaths.Contains(currentSection))
            activePaths.Enqueue(currentSection);
        
        if(activePaths.Count > 10)
        {
            Destroy(activePaths.Peek().gameObject);
            activePaths.Dequeue();
        }
    }
    void TransformSection(int angle, PathSection curSection, PathSection nextSection)
    {
        Vector3 offset = Vector3.zero;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        nextSection.transform.rotation = curSection.transform.rotation * rotation;
        Debug.Log($"Angle: {angle}");

        switch (pathDirection)
        {
            case PathDirection.Left:
                offset = new Vector3(-1.5f,0,hexLength);
                break;
            case PathDirection.Straight:
                offset = new Vector3(0,0,hexLength*2);
                break;
            case PathDirection.Right:
                offset = new Vector3(1.5f,0,hexLength);
                break;
        }
        nextSection.transform.position = curSection.transform.position + offset;
        currentSection = nextSection;
        previousOffset = offset;
    }

    void CalculateAvailablePaths()
    {
        Debug.Log("current Directions: "+ currentSection.GetDirection());
        if (currentSection.GetDirection() == TransitionDirection.Left)
        {
            availablePathSections.RemoveAll(section => section.GetDirection() == TransitionDirection.Left);
            if(!availablePathSections.Intersect(rightSections).Any())
                availablePathSections.AddRange(rightSections);

            Debug.Log("kicked left");
        }
        else if (currentSection.GetDirection() == TransitionDirection.Right)
        {
            availablePathSections.RemoveAll(section => section.GetDirection() == TransitionDirection.Right);
            if(!availablePathSections.Intersect(leftSections).Any())
                availablePathSections.AddRange(leftSections);

            Debug.Log("kicked right");
        }
    }
    void AddToFolder(PathSection ps) => ps.transform.SetParent(GameObject.Find("Path Sections Folder").transform);

    IEnumerator GenerateTest()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            GenerateSection();
        }
    }
}